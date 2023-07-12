/// <reference path="./typings/_reference.d.ts" />
var ErrorHandlerClass = (function () {
    function ErrorHandlerClass() {
    }
    ErrorHandlerClass.prototype.CheckJsonRes = function (data) {
        if (!data) {
            MessageBox.ShowError("Operation failed");
            return false;
        }
        if (data.success == undefined) {
            // data not contains success field
            return true;
        }
        if (!data.success) {
            if (data.message) {
                MessageBox.ShowError(data.message + data.stackTrace);
            }
            else {
                MessageBox.ShowError("Operation failed");
            }
            return false;
        }
        return true;
    };
    return ErrorHandlerClass;
}());
var ErrorHandler = new ErrorHandlerClass();
