/// <reference path="./typings/_reference.d.ts" />
/// <reference path="./typings/angular/angular.d.ts" />
/// <reference path="./UI/MessageBox.ts" />
/// <reference path="./ErrorHandler.ts" />
/// <reference path="./SimGallery/SimGalleryController.ts" />

class AppClass
{
    'use strict';

    constructor()
    {
        var self = this;
        $("#btn-login").click(function ()
        {
            $("#modal-login-dialog").modal();
        });
        $("#btn-login-ok").click(function ()
        {
            self.Login();
        });
        
        $("#btn-register").click(function ()
        {
            $("#modal-signup-dialog").modal();
        });
        $("#btn-register-ok").click(function ()
        {
            self.Register();
        });
        $("#btn-signout").click(function ()
        {
            self.SignOut();
        });

        $(".btn-need-auth").click(function ()
        {
            MessageBox.ShowError("Please Login or Sign up.");
        });

        $(".coming-soon").click(function () {
            MessageBox.ShowMessage("COMING SOON.");
        });

        $(".only-ie").click(function ()
        {
            MessageBox.ShowError("Please use Internet Explorer OR download 3D CAD Installer.");
        });

        $("#id-open-gallery-setup, #menu-navigation-result-link").click(function (evt) {
            evt.preventDefault();
            $("#modal-sim-gallery-dialog").modal();
            SimGalleryController.Instance.Update();
        });
    }

    private Register()
    {
        var email = $("#register-username").val();
        if (email == "" || !email)
        {
            MessageBox.ShowError("Email is empty");
            return;
        }

        var password = $("#register-password").val();
        if (password == "" || !password)
        {
            MessageBox.ShowError("Password is empty");
            return;
        }

        $.post("/Login/Register?email=" + email + "&password=" + password,
            function (data)
            {
                if (!ErrorHandler.CheckJsonRes(data))
                {
                    return;
                }
                window.location.reload(true);
            });
    }

    private SignOut()
    {
        $.post("/Login/SignOut",
            function (data)
            {
                if (!ErrorHandler.CheckJsonRes(data))
                {
                    return;
                }
                window.location.reload(true);
            });
    }

    private Login()
    {
        var userName = $("#login-username").val();
        if (userName == "" || !userName)
        {
            MessageBox.ShowError("Username is empty");
            return;
        }

        var password = $("#login-password").val();
        if (password == "" || !password)
        {
            MessageBox.ShowError("Password is empty");
            return;
        }

        $.post("/Login/Login?name=" + userName + "&password=" + password,
            function (data)
            {
                if (!ErrorHandler.CheckJsonRes(data))
                {
                    return;
                }
                window.location.reload(true);
            });
    }

}

var App: AppClass;