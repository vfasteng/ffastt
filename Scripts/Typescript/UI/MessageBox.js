/// <reference path="../typings/_reference.d.ts" />
var MessageBoxClass = (function () {
    function MessageBoxClass() {
        this._container = $("#modal-message-dialog");
        this._title = $("#modal-message-dialog .modal-title");
        this._text = $("#modal-message-dialog .modal-message-text");
    }
    MessageBoxClass.prototype.ShowMessage = function (msg) {
        this._text.html(msg);
        this._title.html("Message");
        this._container.modal();
    };
    MessageBoxClass.prototype.ShowError = function (msg) {
        this._text.html(msg);
        this._title.html("Error");
        this._container.modal();
    };
    return MessageBoxClass;
}());
var MessageBox;
$(document).ready(function () {
    MessageBox = new MessageBoxClass();
});
