import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Address, RegisterResult, User } from '../../shared/models/user';
import { tap } from 'rxjs';
import { SignalrService } from './signalr.service';

@Injectable({
  providedIn: 'root',
})
export class AccountsService {
  private baseUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);
  private signalrService = inject(SignalrService);

  currentUser = signal<User | null>(null);

  login(values: any) {
    let params = new HttpParams();
    params = params.append('useCookies', 'true');

    return this.httpClient
      .post<User>(`${this.baseUrl}/accounts/login`, values, {
        params,
        withCredentials: true,
      })
      .pipe(
        tap((user) => {
          if (user) {
            this.signalrService.createHubConnection();
          }
        })
      );
  }

  register(values: any) {
    return this.httpClient.post<RegisterResult>(
      `${this.baseUrl}/accounts/custom-register`,
      { registerDto: values }
    );
  }

  getUserInfo() {
    return this.httpClient
      .get<User>(`${this.baseUrl}/accounts/user-info`, {
        withCredentials: true,
      })
      .pipe(
        tap((data) => {
          console.log({ userInfo: data });
          this.currentUser.set(data);
        })
      );
  }

  logout() {
    return this.httpClient
      .post(
        `${this.baseUrl}/accounts/logout`,
        {},
        {
          withCredentials: true,
        }
      )
      .pipe(
        tap(() => {
          this.signalrService.stopHubConnection();
        })
      );
  }

  addOrUpdateAddress(addressDto: Address) {
    return this.httpClient
      .post<Address>(
        `${this.baseUrl}/accounts/address`,
        {
          addressDto,
        },
        {
          withCredentials: true,
        }
      )
      .pipe(
        tap((addressDto) => {
          console.log({ addressDto });
          this.currentUser.update((state) => {
            if (state) {
              state.address = addressDto;
            }
            return state;
          });
        })
      );
  }

  getAuthState() {
    return this.httpClient
      .get<{ isAuthenticated: boolean }>(
        `${this.baseUrl}/accounts/auth-status`,
        {
          withCredentials: true,
        }
      )
      .pipe(
        tap((data) => {
          console.log({ getAuthState: data });
        })
      );
  }
}
