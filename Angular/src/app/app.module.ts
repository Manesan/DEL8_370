import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
//google maps import
import { AgmCoreModule } from '@agm/core';
import { Ng5SliderModule } from 'ng5-slider';
import { CountryCodes } from '../assets/CountryCodes';
import { InterceptorService } from '../environments/interceptor.service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { BrowsePropertiesComponent } from './2_Client/browse-properties/browse-properties.component';
import { ContactAgentComponent } from './2_Client/contact-agent/contact-agent.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { LoginComponent } from './1_User/login/login.component';
import { PointOfInterestComponent } from './6_Property_Administration/point-of-interest/point-of-interest.component';
import { AdminPortalComponent } from './admin-portal/admin-portal.component';
import { RegisterClientComponent } from './2_Client/register-client/register-client.component';
import { ResetPasswordComponent } from './1_User/reset-password-otp/reset-password.component';
import { ValuationsComponent } from './5_Valuation_Administration/valuations/valuations.component';
import { UploadPropertyDocsComponent } from './6_Property_Administration/upload-property-docs/upload-property-docs.component';
import { ResetPasswordNewComponent } from './1_User/reset-password-new/reset-password-new.component';
import { UpdateClientInfoComponent } from './2_Client/update-client-info/update-client-info.component';
import { ViewPropertyComponent } from './2_Client/view-property/view-property.component';
import { ViewClientRentalsComponent } from './2_Client/view-client-rentals/view-client-rentals.component';
import { ViewClientPurchasesComponent } from './2_Client/view-client-purchases/view-client-purchases.component';
import { SearchPropertiesComponent } from './2_Client/search-properties/search-properties.component';
import { InspectionsComponent } from './4_Inspection_Administration/inspections/inspections.component';
import { CitiesComponent } from './10_Location/cities/cities.component';
import { ProvincesComponent } from './10_Location/provinces/provinces.component';
import { SuburbsComponent } from './10_Location/suburbs/suburbs.component';
import { MarketTypesComponent } from './6_Property_Administration/market-types/market-types.component';
import { SpaceTypesComponent } from './6_Property_Administration/space-types/space-types.component';
import { FeatureComponent } from './6_Property_Administration/feature/feature.component';
import { OtherBuildingDetailComponent } from './6_Property_Administration/other-building-detail/other-building-detail.component';
import { ClientComponent } from './7_Client_Administration/client/client.component';
import { EmployeeComponent } from './8_Employee_Administration/employee/employee.component';
import { ReportingComponent } from './9_Reporting_Administration/reporting/reporting.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSelectModule } from '@angular/material/select';
import { ToastrModule } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { DefectComponent } from './6_Property_Administration/defect/defect.component';
import { SpaceComponent } from './6_Property_Administration/space/space.component';
import { PropertyTypeComponent } from './6_Property_Administration/property-type/property-type.component';
import { RentalApplicationsComponent } from './3_Agent/rental-applications/rental-applications.component';
import { PropertyComponent } from './3_Agent/property/property.component';
import { RentalAgreementExtensionsComponent } from './3_Agent/rental-agreement-extensions/rental-agreement-extensions.component';
import { AgentSaleAgreementsComponent } from './3_Agent/agent-sale-agreements/agent-sale-agreements.component';
import { AgentPurchaseOffersComponent } from './3_Agent/agent-purchase-offers/agent-purchase-offers.component';
import { PropertyOwnerComponent } from './3_Agent/property-owner/property-owner.component';
import { InspectionReportComponent } from './9_Reporting_Administration/inspection-report/inspection-report.component';
import { PropertyReportComponent } from './9_Reporting_Administration/property-report/property-report.component';
import { ValuationReportComponent } from './9_Reporting_Administration/valuation-report/valuation-report.component';
import { AgentReportComponent } from './9_Reporting_Administration/agent-report/agent-report.component';
import { SettingsComponent } from './settings/settings.component';
import { NbThemeModule, NbLayoutModule, NbPopoverModule, NbSelectModule } from '@nebular/theme';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import {MatTooltipModule} from '@angular/material/tooltip';
import { NgxCurrencyModule } from "ngx-currency";
import { AuditReportComponent } from './9_Reporting_Administration/audit-report/audit-report.component';
import { PasswordStrengthMeterModule } from 'angular-password-strength-meter';
import { MatPasswordStrengthModule } from '@angular-material-extensions/password-strength';
import { NgxSpinnerModule } from "ngx-spinner";

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    BrowsePropertiesComponent,
    ContactAgentComponent,
    AboutUsComponent,
    LoginComponent,
    PointOfInterestComponent,
    AdminPortalComponent,
    RegisterClientComponent,
    ResetPasswordComponent,
    ValuationsComponent,
    UploadPropertyDocsComponent,
    ResetPasswordNewComponent,
    UpdateClientInfoComponent,
    ViewPropertyComponent,
    ViewClientRentalsComponent,
    ViewClientPurchasesComponent,
    SearchPropertiesComponent,
    InspectionsComponent,
    CitiesComponent,
    ProvincesComponent,
    SuburbsComponent,
    MarketTypesComponent,
    SpaceTypesComponent,
    FeatureComponent,
    OtherBuildingDetailComponent,
    ClientComponent,
    EmployeeComponent,
    ReportingComponent,
    DefectComponent,
    SpaceComponent,
    PropertyTypeComponent,
    RentalApplicationsComponent,
    PropertyComponent,
    RentalAgreementExtensionsComponent,
    AgentSaleAgreementsComponent,
    AgentPurchaseOffersComponent,
    PropertyOwnerComponent,
    InspectionReportComponent,
    PropertyReportComponent,
    ValuationReportComponent,
    AgentReportComponent,
    SettingsComponent,
    AuditReportComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    PasswordStrengthMeterModule,
    //google  api key
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyAzSnXXXXXXXXXXXXXXXXXSZGGWU',
      libraries: ['places']
    }),
    BrowserAnimationsModule,
    MatSelectModule,
    ToastrModule.forRoot(),
    CommonModule,
    ReactiveFormsModule,
    Ng5SliderModule,
    NbThemeModule.forRoot({ name: 'dark' }),
    NbLayoutModule,
    NbEvaIconsModule,
    NbPopoverModule,
    MatTooltipModule,
    ReactiveFormsModule,
    FormsModule,
    NgxCurrencyModule,
    MatPasswordStrengthModule,
    NgxSpinnerModule,
    NbSelectModule
  ],
  providers: [NavBarComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
