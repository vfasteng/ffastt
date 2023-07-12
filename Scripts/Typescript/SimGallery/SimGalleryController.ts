/// <reference path="../typings/_reference.d.ts" />
/// <reference path="../typings/angular/angular.d.ts" />
/// <reference path="../ErrorHandler.ts" />
/// <reference path="../UI/MessageBox.ts" />
/// <reference path="./SimGallerySliderController.ts" />

class SimGalleryController
{
    public static $inject = [
        '$scope'
    ];

    public static Instance: SimGalleryController;

    public GalleryItems: Array<GalleryItem>;

    public IsLoading: boolean;

    private _scope: ng.IScope;

    constructor(private $scope: ng.IScope)
    {
        if (SimGalleryController.Instance)
            throw new Error("Double instance SimGalleryController");

        this._scope = $scope;

        // 'vm' stands for 'view model'. We're adding a reference to the controller to the scope
        // for its methods to be accessible from view / HTML
        (<any>$scope).vm = this;

        this.GalleryItems = null;

        SimGalleryController.Instance = this;
    }

    public Update()
    {
        this.UpdateGalleryList();
    }

    private UpdateGalleryList()
    {
        var self = this;

        self._scope.$apply(function ()
        {
            self.GalleryItems = null;
            self.IsLoading = true;
        });

        $.getJSON("/Gallery/GetItems",
            function (data)
            {
                self._scope.$apply(function ()
                {
                    self.IsLoading = false;
                });

                if (!ErrorHandler.CheckJsonRes(data))
                {
                    return;
                }

                self._scope.$apply(function () {
                    self.GalleryItems = data;
                });

            });
    }

    private Accept()
    {
        var self = this;

        if (self.GalleryItems == null)
        {
            return;
        }

        $.post("/Gallery/SaveItemsStates",
            JSON.stringify(self.GalleryItems),
            function (data)
            {
                if (!ErrorHandler.CheckJsonRes(data))
                {
                    return;
                }

                SimGallerySliderController.Instance.Update();
            });
    }
} 

class GalleryItem
{
    Id: number;

    Name: string;

    Path: string;

    IsEnabled: boolean;
}
