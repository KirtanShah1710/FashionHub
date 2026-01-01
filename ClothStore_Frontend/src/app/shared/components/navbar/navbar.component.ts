import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(
    public authService: AuthService,
    private router: Router
  ) {}

  get userName(): string | null {
    return this.authService.getUserName();
  }

  get role(): string | null {
    return this.authService.getRole();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}
