"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var app_component_1 = require("./app.component");
var angular2_datatable_1 = require("angular2-datatable");
var http_1 = require("@angular/http");
var forms_1 = require("@angular/forms");
var router_1 = require("@angular/router");
var animations_1 = require("@angular/platform-browser/animations");
var angular2_busy_1 = require("angular2-busy");
var xont_ventura_message_prompt_1 = require("xont-ventura-message-prompt");
var xont_ventura_single_prompt_1 = require("xont-ventura-single-prompt");
var xont_ventura_datepicker_1 = require("xont-ventura-datepicker");
var xont_ventura_gridexport_1 = require("xont-ventura-gridexport");
var xont_ventura_validators_1 = require("xont-ventura-validators");
var xont_ventura_services_1 = require("xont-ventura-services");
var xont_ventura_collapsible_1 = require("xont-ventura-collapsible");
var xont_ventura_gridloader_1 = require("xont-ventura-gridloader");
var xont_ventura_list_prompt_1 = require("xont-ventura-list-prompt"); //V3001Added
var xont_ventura_date_controller_1 = require("xont-ventura-date-controller"); //V3001Added
var list_component_1 = require("./list.component/list.component");
var new_component_1 = require("./new.component/new.component");
var userProfile_service_1 = require("./userProfile.service");
var routes = [
    { path: '', redirectTo: '/list', pathMatch: 'full' },
    { path: 'list', component: list_component_1.ListComponent },
    { path: 'new', component: new_component_1.NewComponent }
];
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule, angular2_datatable_1.DataTableModule, http_1.HttpModule, forms_1.FormsModule, router_1.RouterModule.forRoot(routes),
                animations_1.BrowserAnimationsModule, angular2_busy_1.BusyModule, xont_ventura_message_prompt_1.XontVenturaMessagePromptModule,
                xont_ventura_single_prompt_1.XontVenturaSinglePromptModule, xont_ventura_datepicker_1.XontVenturaDatepickerModule,
                xont_ventura_gridexport_1.XontVenturaGridExportModule, xont_ventura_validators_1.XontVenturaValidatorsModule,
                xont_ventura_collapsible_1.XontVenturaCollapsibleModule, xont_ventura_gridloader_1.XontVenturaGridLoaderModule,
                xont_ventura_list_prompt_1.XontVenturaListPromptModule, xont_ventura_date_controller_1.XontVenturaDateControllerModule //V3001Added
            ],
            declarations: [app_component_1.AppComponent, list_component_1.ListComponent, new_component_1.NewComponent],
            bootstrap: [app_component_1.AppComponent],
            providers: [
                xont_ventura_services_1.DatetimeService, xont_ventura_services_1.CommonService, userProfile_service_1.UserProfileService
            ]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map