import { Component, OnInit } from '@angular/core';
import { HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-user-setting',
  templateUrl: './UserSetting.component.html',
  styleUrls: ['./UserSetting.component.css']
})
export class UserSettingComponent implements OnInit {

  values: any;

  constructor( private http: HttpClient) { }

  ngOnInit() {
    this.getAllValues();
  }

  getAllValues(){
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
      this.values = response;
    }, error =>{
      console.log(error);
    });
  }

}
