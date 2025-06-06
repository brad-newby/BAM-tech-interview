import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NewDutyDto } from '../Dtos/NewDutyDto';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseUrl = 'https://localhost:7204'

  constructor(private http: HttpClient) {}

  //Service Calls
  GetAstronautInfo(name: string): Observable<any> {
    const url = `https://localhost:7204/AstronautDuty/${name}`
    return this.http.get(url);
  }

  UpdateAstronautDuty(duty: NewDutyDto): Observable<any> {
    const url = 'https://localhost:7204/AstronautDuty';
    return this.http.post(url, duty);
  }

  CreateNewPerson(newName: string): Observable<any>{
    const url = 'https://localhost:7204/Person';
    const body = {name: newName};
    return this.http.post(url,body);
  }

  GetAllPeople(): Observable<any> {
    const url = 'https://localhost:7204/Person';
    return this.http.get(url);
  }
}
