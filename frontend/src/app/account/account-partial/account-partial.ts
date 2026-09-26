import { Component, computed, inject } from '@angular/core';
import { BffService } from '../../bff/bff-service';

@Component({
  imports: [],
  selector: 'app-account-partial',
  styleUrl: './account-partial.scss',
  templateUrl: './account-partial.html',
})
export class AccountPartial {
  bffService = inject(BffService);

  userResource = this.bffService.getUser();

  // Create a computed signal based on the result of the resource's loader function.
  name = computed(() => {
    if (this.userResource.hasValue()) {
      // `hasValue` serves 2 purposes:
      // - It acts as type guard to strip `undefined` from the type
      // - It protects against reading a throwing `value` when the resource is in error state
      return this.userResource.value().find(user => user.type === 'name')?.value;
    }
    // fallback in case the resource value is `undefined` or if the resource is in error state
    return undefined;
  });

  // Create a computed signal based on the result of the resource's loader function.
  logout_url = computed(() => {
    if (this.userResource.hasValue()) {
      // `hasValue` serves 2 purposes:
      // - It acts as type guard to strip `undefined` from the type
      // - It protects against reading a throwing `value` when the resource is in error state
      return this.userResource.value().find(user => user.type === 'bff:logout_url')?.value;
    }
    // fallback in case the resource value is `undefined` or if the resource is in error state
    return undefined;
  });

  // Create a computed signal based on the result of the resource's loader function.
  session_expires_in = computed(() => {
    if (this.userResource.hasValue()) {
      // `hasValue` serves 2 purposes:
      // - It acts as type guard to strip `undefined` from the type
      // - It protects against reading a throwing `value` when the resource is in error state
      // Assuming session_expires_in is 3600 seconds (1 hour)
      const sessionExpiresIn = this.userResource.value().find(user => user.type === 'bff:session_expires_in')?.value;
      // Calculate expiration date relative to the current time
      const expiryDatetime = new Date(Date.now() + Number(sessionExpiresIn)  * 1000);
      return expiryDatetime;

    }
    // fallback in case the resource value is `undefined` or if the resource is in error state
    return undefined;
  });

}
