import { AlertifyService } from './../../_services/alertify.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../_models/User';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;
  @ViewChild('editForm') editForm: NgForm;
  constructor(private route: ActivatedRoute, private alertifyService: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
  }

  updateUser() {
    console.log(this.user);
    this.alertifyService.success('Profile saved successfully');
    this.editForm.reset(this.user);
  }
}
