/// <reference path="./typings/_reference.d.ts" />
/// <reference path="./typings/angular/angular.d.ts" />
/// <reference path="./UI/MessageBox.ts" />
/// <reference path="./ErrorHandler.ts" />
var SignUpClass = (function () {
    function SignUpClass() {
        var self = this;
        $("#btn-register-ok").click(function () {
            self.Register();
        });
    }
    SignUpClass.prototype.Register = function () {
        var email = $("#register-username").val();
        if (email == "" || !email) {
            MessageBox.ShowError("Email is empty");
            return;
        }
        var password = $("#register-password").val();
        if (password == "" || !password) {
            MessageBox.ShowError("Password is empty");
            return;
        }
        $("#btn-register-ok").prop("disabled", true);
        $.post("/Login/Register?email=" + email + "&password=" + password, function (data) {
            $("#btn-register-ok").prop("disabled", false);
            if (!ErrorHandler.CheckJsonRes(data)) {
                return;
            }
            window.location.href = "/";
        });
    };
    return SignUpClass;
}());
var SignUp;
$(document).ready(function () {
    SignUp = new SignUpClass();
});
// Init angular
var todomvc = angular.module('featanalize', []);
