"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
//import { LocalStorageService } from 'ng2-webstorage';
var common_1 = require("@angular/common");
var router_1 = require("@angular/router");
var xont_ventura_services_1 = require("xont-ventura-services");
var userProfile_service_1 = require("../userProfile.service");
var ListComponent = /** @class */ (function () {
    function ListComponent(location, router, commonService, userProfileService) {
        var _this = this;
        this.location = location;
        this.router = router;
        this.commonService = commonService;
        this.userProfileService = userProfileService;
        this.selectionCriteria = {
            BusinessUnit: '',
            UserProfile: '',
            Name: '',
            RowStart: 1,
            RowEnd: 20,
            //isStartWith: true,
            isStartWith: false,
            isActiveOnly: true,
            Collapsed: true
        };
        this.RestrictFlag = '';
        this.loadingButton = {
            CurrentPage: 0, TotalPage: 0
        };
        this.rowsOnPage = 10;
        this.sortBy = "RouteCode";
        this.sortOrder = "asc";
        this.userProfileService.componentMethodCalled$
            .subscribe(function (error) {
            _this.msgPrompt.show(error, 'SOMNT02');
        });
    }
    ListComponent.prototype.ngOnInit = function () {
        if (localStorage.getItem('SOMNT02_SelectionCriteria') != null) {
            this.selectionCriteria = JSON.parse(localStorage.getItem('SOMNT02_SelectionCriteria'));
            localStorage.removeItem('SOMNT02_SelectionCriteria');
        }
        this.list(true);
    };
    ListComponent.prototype.siteName = function () {
        return this.commonService.getAPIPrefix('SOMNT02');
    };
    ListComponent.prototype.list = function (isInit) {
        var _this = this;
        this.gridLoader.init('SOMNT02');
        this.rowsOnPage = this.gridLoader.getPageSize();
        if (isInit) {
            this.gridLoader.setCurrentPage(1);
            this.selectionCriteria.RowStart = 1;
            console.log(this.gridLoader.getLoadSize());
            this.selectionCriteria.RowEnd = this.gridLoader.getLoadSize();
        }
        else {
            this.selectionCriteria.RowStart = this.gridLoader.getRowStart();
            this.selectionCriteria.RowEnd = this.gridLoader.getRowEnd();
        }
        this.busy = this.userProfileService.getAllUserProfiles(this.selectionCriteria)
            .subscribe(function (jsonData) {
            _this.userProfiles = jsonData[0];
            _this.gridLoader.setRowCount(jsonData[1]);
        });
    };
    ListComponent.prototype.newBasedOn_OnClick = function (item) {
        localStorage.setItem('SOMNT02_PageInit', JSON.stringify({ Mode: 'newBasedOn', UserProfile: item.UserProfile.trim() }));
        this.router.navigate(['new']);
    };
    ListComponent.prototype.edit_OnClick = function (item) {
        localStorage.setItem('SOMNT02_PageInit', JSON.stringify({ Mode: 'edit', UserProfile: item.UserProfile.trim() }));
        localStorage.setItem('SOMNT02_SelectionCriteria', JSON.stringify(this.selectionCriteria));
        this.router.navigate(['new']);
    };
    ListComponent.prototype.btnNew_OnClick = function () {
        localStorage.setItem('SOMNT02_PageInit', JSON.stringify({ Mode: 'new', UserProfile: '' }));
        this.router.navigate(['new']);
    };
    ListComponent.prototype.gridLoader_OnChange = function (data) {
        this.list(false);
    };
    ListComponent.prototype.showError = function (err) {
        this.msgPrompt.show(err.json(), 'SOMNT02');
    };
    __decorate([
        core_1.ViewChild('msgPrompt'),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "msgPrompt", void 0);
    __decorate([
        core_1.ViewChild('gridLoader'),
        __metadata("design:type", Object)
    ], ListComponent.prototype, "gridLoader", void 0);
    ListComponent = __decorate([
        core_1.Component({
            selector: 'my-list',
            templateUrl: './app/list.component/list.component.html',
            styles: ['.deactiveRow {background-color: lightgrey;}'],
        }),
        __metadata("design:paramtypes", [common_1.Location, router_1.Router, xont_ventura_services_1.CommonService, userProfile_service_1.UserProfileService])
    ], ListComponent);
    return ListComponent;
}());
exports.ListComponent = ListComponent;
//# sourceMappingURL=list.component.js.map