/// <reference path="./typings/_reference.d.ts" />
/// <reference path="./typings/angular/angular.d.ts" />
// <reference path="./typings/uploader/ajaxUploder.d.ts" />
/// <reference path="./UI/MessageBox.ts" />
/// <reference path="./ErrorHandler.ts" />


class StorageController
{
    // $inject annotation.
    // It provides $injector with information about dependencies to be injected into constructor
    // it is better to have it close to the constructor, because the parameters must match in count and type.
    // See http://docs.angularjs.org/guide/di
    public static $inject = [
        '$scope'
    ];

    private _scope: ng.IScope;

    private _currentFolder: string;

    public Files: Array<FileDesc>;

    public IsLoading: boolean;

    public IsLoadingFolder: boolean;
    
    public StorageUsageSpaceText: string;

    private _uploaderNewFile;

    constructor(private $scope: ng.IScope)
    {
        this._scope = $scope;

        // 'vm' stands for 'view model'. We're adding a reference to the controller to the scope
        // for its methods to be accessible from view / HTML
        (<any>$scope).vm = this;

        $("#main-container").removeClass("hide");

        this.UpdateInformation();

        this.InitUploadBtn();
        this.UpdateFolderTreeView();
    }

    private UpdateFolderTreeView(currentFolder: string = "")
    {
        var self = this;

        $(document).ready(function ()
        {
            self._scope.$apply(function ()
            {
                self.IsLoadingFolder = true;
            });

            $.getJSON("/Storage/GetFolders", function (folderData)
            {
                self._scope.$apply(function ()
                {
                    self.IsLoadingFolder = false;
                });

                if (!ErrorHandler.CheckJsonRes(folderData))
                {
                    return;
                }

                var root = {
                    nodes: folderData,
                    path: "",
                    text: "root",
                };

                var data = [root];

                var treeDiv = $('#treeFolder');
                (<any>treeDiv).treeview({ data: data });
                treeDiv.on('nodeSelected', function (event, data)
                {
                    self.UpdateFileList(data.path);
                });
                var res = { index: 0 };
                var folderNumber = self.findForldersNumber(data, currentFolder, res);
                if (!folderNumber)
                    folderNumber = 0;
                var node = (<any>treeDiv).treeview("getNode", folderNumber);
                while (node.parentId) {
                    (<any>treeDiv).treeview("expandNode", node.parentId);
                    node = (<any>treeDiv).treeview("getNode", node.parentId);
                }
                (<any>treeDiv).treeview("selectNode", folderNumber);
                
            });
        });
    }

    private findForldersNumber(folders, path: string, res)
    {
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
                };
            }
        });
        return findedIndex;
    }

    private InitUploadBtn()
    {
        var self = this;

        $(document).ready(function ()
        {
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
                startXHR: function ()
                {
                    progressOuter.style.display = 'block'; // make progress bar visible
                    this.setProgressBar(progressBar);
                },
                onSubmit: function ()
                {
                    btn.innerHTML = 'Uploading...'; // change button text to "Uploading..."
                },
                onComplete: function (filename, response)
                {
                    btn.innerHTML = originBtnText;
                    progressOuter.style.display = 'none';

                    self.CompleteUploadFile();
                },
                onError: function ()
                {
                    progressOuter.style.display = 'none';
                    MessageBox.ShowError("Unable to upload file");
                }
            });
        });
    }

    private CompleteUploadFile()
    {
        var self = this;

        self._scope.$apply(function ()
        {
            self.IsLoading = true;
        });        

        self.UpdateFileList(self._currentFolder);
    }

    private UpdateFileList(folder: string)
    {
        var self = this;
        
        self._currentFolder = folder;

        self._uploaderNewFile._opts.url = '/Storage/UploadFile?folder=' + self._currentFolder;

        if (!self._scope.$$phase)
        {
            self._scope.$apply(function ()
            {
                self.IsLoading = true;
            });
        }
        else
        {
            self.IsLoading = true;
        }        
            
        $.getJSON("/Storage/GetFiles?folder=" + folder,
            function (data)
            {
                self.IsLoading = false;

                if (!ErrorHandler.CheckJsonRes(data))
                {
                    return;
                }

                self._scope.$apply(function ()
                {
                    $.each(data, function (index_, item) {
                        item.Name = item.Name.split("/").pop();
                    });
                    self.Files = data;
                });
            });
    }

    private UpdateInformation()
    {
        var self = this;

        self.StorageUsageSpaceText = "...";

        $.getJSON("/Storage/GetInformation",
            function (data)
            {
                if (!ErrorHandler.CheckJsonRes(data))
                {
                    return;
                }

                self._scope.$apply(function ()
                {
                    self.StorageUsageSpaceText = data.UsedSpace.toString() + " / " + data.AvailSpace + " GB";
                });
            });
    }

    private Download(file: FileDesc)
    {
        window.location.href = "/Storage/Download?fileId=" + file.FilePath;
    }

    private Delete(file: FileDesc)
    {
        var self = this;

        self.IsLoading = true;

        $.post("/Storage/Delete?fileId=" + file.FilePath,
            function (data)
            {
                if (!ErrorHandler.CheckJsonRes(data))
                {
                    self._scope.$apply(function ()
                    {
                        self.IsLoading = false;
                    });

                    return;
                }

                self.UpdateFileList(self._currentFolder);
            });
    }

    private GotoCad(file: FileDesc)
    {
        window.location.href = "http://cadfea.cloudapp.net/?file=" + file.FilePath + "&command=openModel";
    }

    private AddFolderDialog()
    {
        $("#folder-name-dialog").modal();
    }

    private AcceptCreateFolderDialog(folderName: string)
    {
        var self = this;
        $.post("/Storage/CreateFolder?name=" + folderName + "&parentFolder=" + self._currentFolder,
            function (data)
            {
                if (!ErrorHandler.CheckJsonRes(data))
                {
                    return;
                }

                self.UpdateFolderTreeView((self._currentFolder ? self._currentFolder + "/" : "") + folderName);
            });
    }

    private DeleteFolder()
    {
        var self = this;

        if (!self._currentFolder)
        {
            MessageBox.ShowError("Folder Not Selected");
            return;
        }

        $.post("/Storage/DeleteFolder?path=" + self._currentFolder,
            function (data)
            {
                if (!ErrorHandler.CheckJsonRes(data))
                {
                    return;
                }

                self.UpdateFolderTreeView();
            });
    }
}

declare module ss
{
    export class SimpleUpload
    {
        constructor(options: any);
    }
}

class FileDesc
{
    Name: string;

    FilePath: string;
}
