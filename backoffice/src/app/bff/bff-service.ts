import { httpResource, HttpResourceRef } from '@angular/common/http';
import { Service, Signal } from '@angular/core';
import { StringDecoder } from 'string_decoder';
import { UserClaim } from './UserClain';

@Service()
export class BffService {
  getUser(
  ) : HttpResourceRef<UserClaim[] | undefined> {
    return httpResource<UserClaim[] | undefined>(() => ({
      url: `/bff/user`,
      method: 'GET',
      headers: {
        'X-CSRF': '1',
      },
    }));
  }

}
