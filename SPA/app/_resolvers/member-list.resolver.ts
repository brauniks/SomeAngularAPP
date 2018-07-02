import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AlertifyService } from './../_services/alertify.service';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { User } from '../_models/User';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';

@Injectable()
export class MemberListResolver implements Resolve<User> {

  constructor(private userService: UserService, private router: Router, private alertify: AlertifyService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    return this.userService.getUsers()
     .catch(error => {
      this.alertify.error('error');
      this.router.navigate(['/home']);
      return Observable.of(null);
    });
  }
}
