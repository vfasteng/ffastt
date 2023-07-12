var AdminController = (function () {
    function AdminController($scope) {
        this.$scope = $scope;
        this.ShowLogo = false;
        this._scope = $scope;
        $scope.vm = this;
        var self = this;
        $(document).ready(function () {
            $.ajax({
                url: "Admin/CheckLogo/",
                dataType: "json"
            })
                .done(function (response) {
                self._scope.$apply(function () {
                    self.ShowLogo = response;
                });
            });
            var btn = document.getElementById('adminFileUploadBtn');
            var progressOuter = document.getElementById('adminFileProgressOuter');
            var progressBar = document.getElementById('adminFileProgressBar');
            var originBtnText = btn.innerHTML;
            // Initilize upload button
            self._uploaderNewFile = new ss.SimpleUpload({
                button: btn,
                url: '/Admin/UploadLogo',
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
                    if (!response.success) {
                        MessageBox.ShowError("Unable to upload file");
                    }
                    else {
                        document.getElementById("admin-logo").setAttribute("src", "Admin/GetLogo/?" + new Date().getTime());
                        self._scope.$apply(function () {
                            self.ShowLogo = true;
                        });
                    }
                    //self.CompleteUploadFile();
                },
                onError: function () {
                    progressOuter.style.display = 'none';
                    MessageBox.ShowError("Unable to upload file");
                }
            });
        });
    }
    return AdminController;
}());
AdminController.$inject = [
    '$scope'
];
