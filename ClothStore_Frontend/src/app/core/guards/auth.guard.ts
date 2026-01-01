import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  // Inject services using the inject function
  const authService = inject(AuthService);
  const router = inject(Router);

  // Check if the user is logged in
  if (authService.isLoggedIn()) {
    return true;
  }

  // Redirect to the login page if not logged in
  router.navigate(['/auth/login']);
  return false;
};
