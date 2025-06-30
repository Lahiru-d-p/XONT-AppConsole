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
var router_1 = require("@angular/router");
var common_1 = require("@angular/common");
var xont_ventura_services_1 = require("xont-ventura-services");
var userProfile_service_1 = require("../userProfile.service");
var NewComponent = /** @class */ (function () {
    function NewComponent(location, userProfileService, router, datetimeService, commonService) {
        var _this = this;
        this.location = location;
        this.userProfileService = userProfileService;
        this.router = router;
        this.datetimeService = datetimeService;
        this.commonService = commonService;
        this.pageInit = undefined;
        this.imageSrc = '';
        this.UserProfile = {
            BusinessUnit: '',
            UserProfileShortName: '',
            UserProfileName: '',
            AddressLine1: '',
            AddressLine2: '',
            AddressLine3: '',
            AddressLine4: '',
            AddressLine5: '',
            PostCode: '',
            TelephoneNumber: '',
            MobileNumber: '',
            EmailAddress: '',
            EmployeeNo: '',
            TradeSchemeGroup: '',
            TradeSchemeGroupDesc: '',
            LastPaymentDate: null,
            Status: '1',
            NIC: '',
            DateOfBirth: null,
            DateOfBirthString: '',
            UserImage: null,
            UserImageStr: '',
            Mode: ''
        };
        //V3001Adding start
        this.nicValid = true;
        this.userProfileService.componentMethodCalled$
            .subscribe(function (error) {
            _this.msgPrompt.show(error, 'SOMNT02');
        });
    }
    NewComponent.prototype.lmptTradeShemeGroup_DataBind = function () {
        this.lmptTradeShemeGroup.dataSourceObservable = this.userProfileService.getTradeSchemeGroupData();
    };
    NewComponent.prototype.ngOnInit = function () {
        var _this = this;
        var node = document.createElement('script');
        node.src = '../js/imageViewer.js';
        node.type = 'text/javascript';
        document.getElementsByTagName('head')[0].appendChild(node);
        this.pageInit = JSON.parse(localStorage.getItem('SOMNT02_PageInit'));
        console.log(this.pageInit);
        if (!this.pageInit) {
            this.router.navigate(['list']);
        }
        if (this.pageInit.Mode == 'edit' || this.pageInit.Mode == 'newBasedOn') {
            this.busy = this.userProfileService.getUserProfile(this.pageInit.UserProfile)
                .subscribe(function (data) {
                _this.UserProfile = data;
                console.log(_this.UserProfile.DateOfBirth);
                _this.UserProfile.DateOfBirthString = _this.UserProfile.DateOfBirth ? _this.datetimeService.getDisplayDate(new Date(_this.UserProfile.DateOfBirth)) : '';
                if (_this.pageInit.Mode == 'newBasedOn') {
                    _this.UserProfile.UserProfileShortName = '';
                }
                if (_this.UserProfile.UserImage) {
                    _this.imageSrc = 'data:image/jpg;base64,' + _this.UserProfile.UserImage;
                }
            });
        }
    };
    NewComponent.prototype.siteName = function () {
        return this.commonService.getAPIPrefix('SOMNT02');
    };
    NewComponent.prototype.btnCancel_Click = function () {
        this.location.back();
    };
    NewComponent.prototype.setStatus = function (status) {
        this.UserProfile.Status = status ? '1' : '0';
    };
    NewComponent.prototype.openfileDialog = function () {
        document.getElementById('avatarUpload').click();
    };
    NewComponent.prototype.removeImage = function () {
        this.imageSrc = '';
    };
    NewComponent.prototype.onSubmit = function () {
        var _this = this;
        this.UserProfile.Mode = (this.pageInit.Mode == 'new' || this.pageInit.Mode == 'newBasedOn') ? '0' : '1';
        this.UserProfile.DateOfBirth = this.datetimeService.getDateTimeForString(this.UserProfile.DateOfBirthString);
        if (this.imageSrc != '') {
            this.UserProfile.UserImageStr = this.imageSrc;
        }
        else {
            this.UserProfile.UserImageStr = null;
        }
        this.UserProfile.UserImage = null;
        this.busy = this.userProfileService.updateUserProfile(this.UserProfile)
            .subscribe(function (data) {
            if (data == true) {
                _this.router.navigate(['list']);
            }
        });
    };
    NewComponent.prototype.showError = function (err) {
        this.msgPrompt.show(err, 'SOMNT02');
    };
    NewComponent.prototype.ValidateNIC = function (nicNumber) {
        console.log('here goes the validting NIC', nicNumber);
        if (!nicNumber) {
            this.nicValid = true;
        }
        else if (nicNumber.length === 10 && !isNaN(nicNumber.substr(0, 9)) && isNaN(nicNumber.substr(9, 1).toLowerCase()) && ['x', 'v'].indexOf(nicNumber.substr(9, 1).toLowerCase())) {
            this.nicValid = true;
        }
        else if (nicNumber.length === 12 && !isNaN(nicNumber)) {
            this.nicValid = true;
        }
        else {
            this.nicValid = false;
        }
    };
    NewComponent.prototype.OnNICChange = function (event) {
        this.ValidateNIC(this.UserProfile.NIC);
    };
    //////////////////////image Upload///////////////////////////////////////
    NewComponent.prototype.handleProfilePictureInputChange = function (e) {
        var file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
        var pattern = /image-*/;
        var reader = new FileReader();
        if (!file.type.match(pattern)) {
            this.msgAlertPrompt.showAlert("Invalid File Format", "OK"); //V3001Added
            //alert('invalid format');V3001Removed
            return;
        }
        reader.onload = this._handleWarrentyReaderLoaded.bind(this);
        reader.readAsDataURL(file);
    };
    NewComponent.prototype._handleWarrentyReaderLoaded = function (e) {
        var reader = e.target;
        this.imageSrc = reader.result;
    };
    __decorate([
        core_1.ViewChild('msgPrompt'),
        __metadata("design:type", Object)
    ], NewComponent.prototype, "msgPrompt", void 0);
    __decorate([
        core_1.ViewChild('lmptTradeShemeGroup'),
        __metadata("design:type", Object)
    ], NewComponent.prototype, "lmptTradeShemeGroup", void 0);
    __decorate([
        core_1.ViewChild('msgAlertPrompt'),
        __metadata("design:type", Object)
    ], NewComponent.prototype, "msgAlertPrompt", void 0);
    NewComponent = __decorate([
        core_1.Component({
            selector: 'my-new',
            templateUrl: './app/new.component/new.component.html',
            styles: ['.selectedRow {background-color: #669999;}'],
        }),
        __metadata("design:paramtypes", [common_1.Location,
            userProfile_service_1.UserProfileService,
            router_1.Router,
            xont_ventura_services_1.DatetimeService,
            xont_ventura_services_1.CommonService])
    ], NewComponent);
    return NewComponent;
}());
exports.NewComponent = NewComponent;
//# sourceMappingURL=new.component.js.map