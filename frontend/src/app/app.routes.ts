import { Routes } from '@angular/router';
import { HomePage } from './home/home-page/home-page';

const homePageRoute = {
  path: '',
  component: HomePage,
  title: 'Home',
};

export const routes: Routes = [
  homePageRoute
];
