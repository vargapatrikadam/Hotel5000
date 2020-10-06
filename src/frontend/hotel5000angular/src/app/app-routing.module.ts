import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AboutComponent } from './components/about/about.component';
import { HomeComponent } from './components/home/home.component'
import { LodgingResolver } from './components/lodgings/lodgings-details/lodging-details-resolver';
import { LodgingsDetailsComponent } from './components/lodgings/lodgings-details/lodgings-details.component';
import { LodgingsMainComponent } from './components/lodgings/lodgings-main/lodgings-main.component';

const routes: Routes = [
  {
    path:'',
    redirectTo:'home',
    pathMatch:'full'
  },
  {
    path:'home',
    component: HomeComponent
  },
  {
    path:'about',
    component: AboutComponent
  },
  {
    path:'lodgings',
    component: LodgingsMainComponent
  },
  {
    path:'lodgings/:id',
    component: LodgingsDetailsComponent,
    resolve: {
      lodgingResult: LodgingResolver
    }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
