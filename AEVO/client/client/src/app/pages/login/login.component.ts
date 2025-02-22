import { Component, Inject, LOCALE_ID } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { UserService } from '../../services/user/user.service';
import { CommonModule } from '@angular/common';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import translationsEN from '../../../../public/i18n/en.json';
import translationsPTBR from '../../../../public/i18n/pt-BR.json';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    TranslatePipe,
  ],
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private userService: UserService,
    @Inject(LOCALE_ID) public activeLocale: string,
    private translate: TranslateService
  ) {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onLogin() {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      const idioma = localStorage.getItem('idioma') || this.activeLocale;

      this.userService
        .login(email, password, idioma)
        .subscribe({
          next: (response) => {
            console.log('Login bem-sucedido!', response);

            localStorage.setItem('accessToken', response.accessToken);
            localStorage.setItem('usuarioId', response.usuarioId);
            localStorage.setItem('idioma', response.idioma);

            // TODO - FIQUEI SEM TEMPO PARA REFATORAR E CRIAR UM LISTENER
            switch (response.idioma) {
              case 'en-US':
                this.translate.setTranslation(response.idioma, translationsEN);
                break;
              default:
                this.translate.setTranslation(response.idioma, translationsPTBR);
                break;
            }

            localStorage.setItem('idioma', response.idioma);
            this.activeLocale = response.idioma;
            this.translate.setDefaultLang(response.idioma);
            this.translate.use(response.idioma);

            this.router.navigate(['/agendamentos']);
          },
          error: (error) => {
            this.errorMessage = 'Usuário ou senha incorretos';
            console.error('Erro ao fazer login:', error);
          },
        });
    }
  }

  goToRegister() {
    this.router.navigate(['/register']);
  }
}
