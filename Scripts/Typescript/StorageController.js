/// <reference path="./typings/_reference.d.ts" />
/// <reference path="./typings/angular/angular.d.ts" />
// <reference path="./typings/uploader/ajaxUploder.d.ts" />
/// <reference path="./UI/MessageBox.ts" />
/// <reference path="./ErrorHandler.ts" />
var StorageController = (function () {
    function StorageController($scope) {
        this.$scope = $scope;
        this._scope = $scope;
        // 'vm' stands for 'view model'. We're adding a reference to the controller to the scope
        // for its methods to be accessible from view / HTML
        $scope.vm = this;
        $("#main-container").removeClass("hide");
        this.UpdateInformation();
        this.InitUploadBtn();
        this.UpdateFolderTreeView();
    }
    StorageController.prototype.UpdateFolderTreeView = function (currentFolder) {
        if (currentFolder === void 0) { currentFolder = ""; }
        var self = this;
        $(document).ready(function () {
            self._scope.$apply(function () {
                self.IsLoadingFolder = true;
            });
            $.getJSON("/Storage/GetFolders", function (folderData) {
                self._scope.$apply(function () {
                    self.IsLoadingFolder = false;
                });
                if (!ErrorHandler.CheckJsonRes(folderData)) {
                    return;
                }
                var root = {
                    nodes: folderData,
                    path: "",
                    text: "root",
                };
                var data = [root];
                var treeDiv = $('#treeFolder');
                treeDiv.treeview({ data: data });
                treeDiv.on('nodeSelected', function (event, data) {
                    self.UpdateFileList(data.path);
                });
                var res = { index: 0 };
                var folderNumber = self.findForldersNumber(data, currentFolder, res);
                if (!folderNumber)
                    folderNumber = 0;
                var node = treeDiv.treeview("getNode", folderNumber);
                while (node.parentId) {
                    treeDiv.treeview("expandNode", node.parentId);
                    node = treeDiv.treeview("getNode", node.parentId);
                }
                treeDiv.treeview("selectNode", folderNumber);
            });
        });
    };
    StorageController.prototype.findForldersNumber = function (folders, path, res) {
        var self = this;
        var findedIndex = null;
        $.each(folders, function (index_, folder) {
            if (folder.path == path) {
                findedIndex = res.index;
            }
            res.index++;
            if (folder.nodes) {
                var result = self.findForldersNumber(folder.nodes, path, res);
                if (result != null) {
                    findedIndex = result;
                }
                ;
            }
        });
        return findedIndex;
    };
    StorageController.prototype.InitUploadBtn = function () {
        var self = this;
        $(document).ready(function () {
            var btn = document.getElementById('storageFileUploadBtn');
            var progressOuter = document.getElementById('storageFileProgressOuter');
            var progressBar = document.getElementById('storageFileProgressBar');
            var originBtnText = btn.innerHTML;
            // Initilize upload button
            self._uploaderNewFile = new ss.SimpleUpload({
                button: btn,
                url: '/Storage/UploadFile',
                name: 'uploadfile',
                hoverClass: 'hover',
                focusClass: 'focus',
                responseType: 'json',
                startXHR: function () {
                    progressOuter.style.display = 'block'; // make progress bar visible
                    this.setProgressBar(progressBar);
                },
                onSubmit: function () {
                    btn.innerHTML = 'Uploading...'; // change button text to "Uploading..."
                },
                onComplete: function (filename, response) {
                    btn.innerHTML = originBtnText;
                    progressOuter.style.display = 'none';
                    self.CompleteUploadFile();
                },
                onError: function () {
                    progressOuter.style.display = 'none';
                    MessageBox.ShowError("Unable to upload file");
                }
            });
        });
    };
    StorageController.prototype.CompleteUploadFile = function () {
        var self = this;
        self._scope.$apply(function () {
            self.IsLoading = true;
        });
        self.UpdateFileList(self._currentFolder);
    };
    StorageController.prototype.UpdateFileList = function (folder) {
        var self = this;
        self._currentFolder = folder;
        self._uploaderNewFile._opts.url = '/Storage/UploadFile?folder=' + self._currentFolder;
        if (!self._scope.$$phase) {
            self._scope.$apply(function () {
                self.IsLoading = true;
            });
        }
        else {
            self.IsLoading = true;
        }
        $.getJSON("/Storage/GetFiles?folder=" + folder, function (data) {
            self.IsLoading = false;
            if (!ErrorHandler.CheckJsonRes(data)) {
                return;
            }
            self._scope.$apply(function () {
                $.each(data, function (index_, item) {
                    item.Name = item.Name.split("/").pop();
                });
                self.Files = data;
            });
        });
    };
    StorageController.prototype.UpdateInformation = function () {
        var self = this;
        self.StorageUsageSpaceText = "...";
        $.getJSON("/Storage/GetInformation", function (data) {
            if (!ErrorHandler.CheckJsonRes(data)) {
                return;
            }
            self._scope.$apply(function () {
                self.StorageUsageSpaceText = data.UsedSpace.toString() + " / " + data.AvailSpace + " GB";
            });
        });
    };
    StorageController.prototype.Download = function (file) {
        window.location.href = "/Storage/Download?fileId=" + file.FilePath;
    };
    StorageController.prototype.Delete = function (file) {
        var self = this;
        self.IsLoading = true;
        $.post("/Storage/Delete?fileId=" + file.FilePath, function (data) {
            if (!ErrorHandler.CheckJsonRes(data)) {
                self._scope.$apply(function () {
                    self.IsLoading = false;
                });
                return;
            }
            self.UpdateFileList(self._currentFolder);
        });
    };
    StorageController.prototype.GotoCad = function (file) {
        window.location.href = "http://cadfea.cloudapp.net/?file=" + file.FilePath + "&command=openModel";
    };
    StorageController.prototype.AddFolderDialog = function () {
        $("#folder-name-dialog").modal();
    };
    StorageController.prototype.AcceptCreateFolderDialog = function (folderName) {
        var self = this;
        $.post("/Storage/CreateFolder?name=" + folderName + "&parentFolder=" + self._currentFolder, function (data) {
            if (!ErrorHandler.CheckJsonRes(data)) {
                return;
            }
            self.UpdateFolderTreeView((self._currentFolder ? self._currentFolder + "/" : "") + folderName);
        });
    };
    StorageController.prototype.DeleteFolder = function () {
        var self = this;
        if (!self._currentFolder) {
            MessageBox.ShowError("Folder Not Selected");
            return;
        }
        $.post("/Storage/DeleteFolder?path=" + self._currentFolder, function (data) {
            if (!ErrorHandler.CheckJsonRes(data)) {
                return;
            }
            self.UpdateFolderTreeView();
        });
    };
    return StorageController;
}());
// $inject annotation.
// It provides $injector with information about dependencies to be injected into constructor
// it is better to have it close to the constructor, because the parameters must match in count and type.
// See http://docs.angularjs.org/guide/di
StorageController.$inject = [
    '$scope'
];
var FileDesc = (function () {
    function FileDesc() {
    }
    return FileDesc;
}());
