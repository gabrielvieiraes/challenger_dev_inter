import { Component, Inject, LOCALE_ID } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { UserService } from '../../services/user/user.service';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { TranslatePipe } from '@ngx-translate/core';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    TranslatePipe,
    MatSelectModule,
    FormsModule,
  ],
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string = '';
  locales = [
    { code: 'en-US', label: 'OPTIONENUS' },
    { code: 'pt-BR', label: 'OPTIONPTBR' },
  ];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private userService: UserService,
    @Inject(LOCALE_ID) public activeLocale: string
  ) {
    const idioma = localStorage.getItem('idioma');
    if (idioma) {
      this.activeLocale = idioma;
    } else {
      this.activeLocale = this.activeLocale || 'pt-BR';
    }

    const userTimezone = Intl.DateTimeFormat().resolvedOptions().timeZone;

    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      nome: ['', Validators.required],
      idioma: [this.activeLocale, Validators.required],
      fusoHorario: [userTimezone, Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    });
  }

  onRegister() {
    const currentLanguage = localStorage.getItem('idioma') || this.activeLocale;

    if (this.registerForm.valid) {
      const { email, nome, idioma, fusoHorario, password, confirmPassword } =
        this.registerForm.value;

      if (password !== confirmPassword) {
        switch (currentLanguage) {
          case 'en-US':
            this.errorMessage = 'Passwords do not match!';
            break;
          default:
            this.errorMessage = 'As senhas não coincidem!';
            break;
        }

        return;
      }

      this.userService
        .register(email, nome, idioma, fusoHorario, password)
        .subscribe({
          next: (response) => {
            console.log('Cadastro realizado com sucesso!', response);
            this.router.navigate(['/login']);
          },
          error: (error) => {
            this.errorMessage = 'Erro ao cadastrar usuário';
            console.error('Erro:', error);
          },
        });
    }
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
}
