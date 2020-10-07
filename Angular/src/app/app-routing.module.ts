import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {AboutUsComponent} from './about-us/about-us.component';
import {AdminPortalComponent} from './admin-portal/admin-portal.component';

import {LoginComponent} from './1_User/login/login.component';
import {ResetPasswordComponent} from './1_User/reset-password-otp/reset-password.component';
import {ResetPasswordNewComponent} from './1_User/reset-password-new/reset-password-new.component';

import {ContactAgentComponent} from './2_Client/contact-agent/contact-agent.component';
import {BrowsePropertiesComponent} from './2_Client/browse-properties/browse-properties.component';
import {RegisterClientComponent} from './2_Client/register-client/register-client.component';
import {UpdateClientInfoComponent} from './2_Client/update-client-info/update-client-info.component';
import {ViewPropertyComponent} from './2_Client/view-property/view-property.component';
import {ViewClientRentalsComponent} from './2_Client/view-client-rentals/view-client-rentals.component';
import {ViewClientPurchasesComponent} from './2_Client/view-client-purchases/view-client-purchases.component';
import {SearchPropertiesComponent} from './2_Client/search-properties/search-properties.component';

import { PropertyOwnerComponent } from './3_Agent/property-owner/property-owner.component';
import {PropertyComponent}from './3_Agent/property/property.component';
import { RentalApplicationsComponent } from './3_Agent/rental-applications/rental-applications.component';
import { AgentPurchaseOffersComponent } from './3_Agent/agent-purchase-offers/agent-purchase-offers.component';
import { RentalAgreementExtensionsComponent } from './3_Agent/rental-agreement-extensions/rental-agreement-extensions.component';
import { AgentSaleAgreementsComponent } from './3_Agent/agent-sale-agreements/agent-sale-agreements.component';


import {InspectionsComponent } from './4_Inspection_Administration/inspections/inspections.component';

import {ValuationsComponent} from './5_Valuation_Administration/valuations/valuations.component';

import {PointOfInterestComponent} from './6_Property_Administration/point-of-interest/point-of-interest.component';
import {UploadPropertyDocsComponent} from './6_Property_Administration/upload-property-docs/upload-property-docs.component';
import {MarketTypesComponent} from './6_Property_Administration/market-types/market-types.component';
import {SpaceTypesComponent} from './6_Property_Administration/space-types/space-types.component';
import {FeatureComponent} from './6_Property_Administration/feature/feature.component';
import {OtherBuildingDetailComponent} from './6_Property_Administration/other-building-detail/other-building-detail.component';
import { DefectComponent } from './6_Property_Administration/defect/defect.component';
import { SpaceComponent } from './6_Property_Administration/space/space.component';
import { PropertyTypeComponent } from './6_Property_Administration/property-type/property-type.component';


import { ClientComponent } from './7_Client_Administration/client/client.component';

import { EmployeeComponent } from './8_Employee_Administration/employee/employee.component';

import { ReportingComponent } from './9_Reporting_Administration/reporting/reporting.component';

import {CitiesComponent} from './10_Location/cities/cities.component';
import {ProvincesComponent} from './10_Location/provinces/provinces.component';
import {SuburbsComponent} from './10_Location/suburbs/suburbs.component';

import { PropertyReportComponent } from './9_Reporting_Administration/property-report/property-report.component';
import { AgentReportComponent } from './9_Reporting_Administration/agent-report/agent-report.component';
import { InspectionReportComponent } from './9_Reporting_Administration/inspection-report/inspection-report.component';
import { ValuationReportComponent } from './9_Reporting_Administration/valuation-report/valuation-report.component';

import { SettingsComponent } from '../app/settings/settings.component';
import { AuditReportComponent } from './9_Reporting_Administration/audit-report/audit-report.component';




const routes: Routes = [
  {path: '', component: SearchPropertiesComponent },
  {path: 'contactagent', component: ContactAgentComponent },
  {path: 'browse', component: BrowsePropertiesComponent },
  {path: 'login', component: LoginComponent },
  {path: 'pointofinterest', component: PointOfInterestComponent },
  {path: 'adminportal', component: AdminPortalComponent },
  {path: 'register', component: RegisterClientComponent },
  {path: 'resetpassword', component: ResetPasswordComponent },
  {path: 'valuations', component: ValuationsComponent },
  {path: 'propertydocs', component: UploadPropertyDocsComponent },
  {path: 'resetpasswordnew', component: ResetPasswordNewComponent },
  {path: 'updateclient', component: UpdateClientInfoComponent },
  {path: 'viewproperty', component: ViewPropertyComponent },
  {path: 'viewproperty/:id', component: ViewPropertyComponent },
  {path: 'client-rentals', component: ViewClientRentalsComponent },
  {path: 'client-purchases', component: ViewClientPurchasesComponent },
  {path: 'search-properties', component: SearchPropertiesComponent },
  {path: 'inspections', component: InspectionsComponent },
  {path: 'cities', component: CitiesComponent },
  {path: 'provinces', component: ProvincesComponent },
  {path: 'suburbs', component: SuburbsComponent },
  {path: 'market-types', component: MarketTypesComponent },
  {path: 'space-types', component: SpaceTypesComponent },
  {path: 'features', component: FeatureComponent },
  {path: 'other-building-details', component: OtherBuildingDetailComponent },
  {path: 'clients', component: ClientComponent },
  {path: 'employees', component: EmployeeComponent },
  {path: 'reporting', component: ReportingComponent },
  {path: 'property', component: PropertyComponent },
  {path: 'propertyowner', component: PropertyOwnerComponent },
  {path: 'rentalapplications', component: RentalApplicationsComponent },
  {path: 'agentpurchases', component: AgentPurchaseOffersComponent },
  {path: 'agentrentalagreements', component: RentalAgreementExtensionsComponent },
  {path: 'agentsaleagreements', component: AgentSaleAgreementsComponent },
  {path: 'propertytype', component: PropertyTypeComponent },
  {path: 'defect', component: DefectComponent },
  {path: 'space', component: SpaceComponent },
  {path: 'property-report', component: PropertyReportComponent },
  {path: 'agent-report', component: AgentReportComponent },
  {path: 'audit-report', component: AuditReportComponent },
  {path: 'inspection-report', component: InspectionReportComponent },
  {path: 'valuation-report', component: ValuationReportComponent },
  {path: 'settings', component: SettingsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
