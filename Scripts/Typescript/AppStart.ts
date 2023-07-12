/// <reference path="./typings/_reference.d.ts" />
/// <reference path="./App.ts" />
/// <reference path="./SimGallery/SimGalleryController.ts" />
/// <reference path="./SimGallery/SimGallerySliderController.ts" />
/// <reference path="./StorageController.ts" />
/// <reference path="./AdminController.ts" />

// Init angular
var todomvc = angular.module('featanalize', [])
    .controller("simCntrl", SimGalleryController)
    .controller("gallerySliderCntrl", SimGallerySliderController)
    .controller('storageCntrl', StorageController)
    .controller("adminCtrl", AdminController);


$(document).ready(function ()
{
    App = new AppClass();
    $(document).ready(function () {
        $.ajaxSetup({ cache: false });
    });
});

