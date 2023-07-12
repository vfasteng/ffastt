/// <reference path="../typings/_reference.d.ts" />
/// <reference path="../typings/angular/angular.d.ts" />
/// <reference path="../ErrorHandler.ts" />
/// <reference path="../UI/MessageBox.ts" />
/// <reference path="./SimGallerySliderController.ts" />
var SimGalleryController = (function () {
    function SimGalleryController($scope) {
        this.$scope = $scope;
        if (SimGalleryController.Instance)
            throw new Error("Double instance SimGalleryController");
        this._scope = $scope;
        // 'vm' stands for 'view model'. We're adding a reference to the controller to the scope
        // for its methods to be accessible from view / HTML
        $scope.vm = this;
        this.GalleryItems = null;
        SimGalleryController.Instance = this;
    }
    SimGalleryController.prototype.Update = function () {
        this.UpdateGalleryList();
    };
    SimGalleryController.prototype.UpdateGalleryList = function () {
        var self = this;
        self._scope.$apply(function () {
            self.GalleryItems = null;
            self.IsLoading = true;
        });
        $.getJSON("/Gallery/GetItems", function (data) {
            self._scope.$apply(function () {
                self.IsLoading = false;
            });
            if (!ErrorHandler.CheckJsonRes(data)) {
                return;
            }
            self._scope.$apply(function () {
                self.GalleryItems = data;
            });
        });
    };
    SimGalleryController.prototype.Accept = function () {
        var self = this;
        if (self.GalleryItems == null) {
            return;
        }
        $.post("/Gallery/SaveItemsStates", JSON.stringify(self.GalleryItems), function (data) {
            if (!ErrorHandler.CheckJsonRes(data)) {
                return;
            }
            SimGallerySliderController.Instance.Update();
        });
    };
    return SimGalleryController;
}());
SimGalleryController.$inject = [
    '$scope'
];
var GalleryItem = (function () {
    function GalleryItem() {
    }
    return GalleryItem;
}());
