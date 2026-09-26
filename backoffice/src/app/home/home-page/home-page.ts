import { Component, inject } from '@angular/core';
import { BffService } from '../../bff/bff-service';

@Component({
  imports: [],
  selector: 'app-home-page',
  styleUrl: './home-page.scss',
  templateUrl: './home-page.html',
})
export class HomePage {
  bffService = inject(BffService);

  user = this.bffService.getUser();
}
