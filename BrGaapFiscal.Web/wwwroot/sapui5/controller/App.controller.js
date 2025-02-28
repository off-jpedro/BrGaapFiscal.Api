sap.ui.define([
    "sap/ui/core/mvc/Controller"
], function (Controller) {
    "use strict";

    return Controller.extend("my.app.controller.App", {
        onInit: function () {
            var oModel = this.getView().getModel("notaFiscal");
            oModel.loadData("/api/notafiscal");
        }
    });
});