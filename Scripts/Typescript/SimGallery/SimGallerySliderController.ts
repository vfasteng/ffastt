/// <reference path="../typings/_reference.d.ts" />
/// <reference path="../typings/angular/angular.d.ts" />
/// <reference path="../ErrorHandler.ts" />
/// <reference path="../UI/MessageBox.ts" />

class SimGallerySliderController
{
    public static $inject = [
        '$scope'
    ];

    public static Instance: SimGallerySliderController;

    public GalleryItems: Array<GalleryItem>;

    public GalleryItemsDefaults: Array<GalleryItem>;

    public IsLoading: boolean;

    public IsShowDefault:boolean;

    private _scope: ng.IScope;

    constructor(private $scope: ng.IScope)
    {
        this._scope = $scope;

        // 'vm' stands for 'view model'. We're adding a reference to the controller to the scope
        // for its methods to be accessible from view / HTML
        (<any>$scope).vm = this;

        this.GalleryItems = null;
        this.IsShowDefault = true;

        SimGallerySliderController.Instance = this;

        this.UpdateGalleryView();
    }

    public Update()
    {
        this.UpdateGalleryView();
    }

    private CreateDefaultItems()
    {
        this.GalleryItemsDefaults = new Array<GalleryItem>();

        var gItem = new GalleryItem();
        gItem.Name = "03.png";
        this.GalleryItemsDefaults.push(gItem);

        var gItem = new GalleryItem();
        gItem.Name = "05.png";
        this.GalleryItemsDefaults.push(gItem);

        var gItem = new GalleryItem();
        gItem.Name = "06.png";
        this.GalleryItemsDefaults.push(gItem);

        var gItem = new GalleryItem();
        gItem.Name = "07.png";
        this.GalleryItemsDefaults.push(gItem);

        var gItem = new GalleryItem();
        gItem.Name = "09.png";
        this.GalleryItemsDefaults.push(gItem);

        var gItem = new GalleryItem();
        gItem.Name = "10.png";
        this.GalleryItemsDefaults.push(gItem);

        var gItem = new GalleryItem();
        gItem.Name = "11.png";
        this.GalleryItemsDefaults.push(gItem);

        var gItem = new GalleryItem();
        gItem.Name = "12.png";
        this.GalleryItemsDefaults.push(gItem);

        var gItem = new GalleryItem();
        gItem.Name = "13.png";
        this.GalleryItemsDefaults.push(gItem);
    }

    private UpdateGalleryView()
    {
        var self = this;

        if (!self.$scope.$$phase)
        {
            self._scope.$apply(function ()
            {
                self.GalleryItems = null;
                self.IsLoading = true;
            });
        }
        else
        {
            self.GalleryItems = null;
            self.IsLoading = true;
        }

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

                self._scope.$apply(function ()
                {
                    var items = <Array<GalleryItem>>data;
                    // Select only enabled
                    items = _.filter(items, (g) => g.IsEnabled);

                    if (items.length > 0)
                    {
                        self.GalleryItems = items;
                        self.GalleryItemsDefaults = null;
                    }
                    else
                    {
                        self.GalleryItems = null;
                        self.CreateDefaultItems();
                    }

                    (<any>$('#horiz_container_outer')).horizontalScroll();
                });
            }).fail(function ()
            {
                // Can't get items for Database - just init
                (<any>$('#horiz_container_outer')).horizontalScroll();
            });
    }
}
