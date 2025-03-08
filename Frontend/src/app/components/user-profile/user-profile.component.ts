import { Component, EventEmitter, Input } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent {

    public userName : string|null = "";
    public userId: string|null = "";

    constructor(
      private auth: AuthService
    ) {
    }

    ngOnInit(): void{
      this.userId = localStorage.getItem('userid');
      this.auth.getUser(this.userId).subscribe(res =>{
        // this.userName = res.body.firstname;
        console.log(res);
      })
    }
}
