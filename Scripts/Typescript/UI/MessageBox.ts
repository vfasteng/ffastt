/// <reference path="../typings/_reference.d.ts" />

class MessageBoxClass
{
    private _container: JQuery;

    private _text: JQuery;

    private _title: JQuery;


    constructor()
    {
        this._container = $("#modal-message-dialog");
        this._title = $("#modal-message-dialog .modal-title");
        this._text = $("#modal-message-dialog .modal-message-text");
    }

    ShowMessage(msg: string)
    {
        this._text.html(msg);
        this._title.html("Message");
        this._container.modal();
    }

    ShowError(msg: string)
    {
        this._text.html(msg);
        this._title.html("Error");
        this._container.modal();
    }
} 

var MessageBox : MessageBoxClass;

$(document).ready(function ()
{
    MessageBox = new MessageBoxClass();
});
