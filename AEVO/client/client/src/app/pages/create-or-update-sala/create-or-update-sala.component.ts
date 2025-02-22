import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { MatSnackBar } from '@angular/material/snack-bar';
import { SalasService } from '../../services/salas/salas.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-create-or-update-sala',
  imports: [
    MatButtonModule,
    MatDialogModule,
    MatIconModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    CommonModule,
    TranslatePipe
  ],
  templateUrl: './create-or-update-sala.component.html',
  styleUrl: './create-or-update-sala.component.css',
})
export class CreateOrUpdateSalaComponent implements OnInit {
  salaForm: FormGroup;
  titulo: string = 'TITLEADICIONARSALA';
  salaId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private salasService: SalasService,
    private snackBar: MatSnackBar
  ) {
    const userTimezone = Intl.DateTimeFormat().resolvedOptions().timeZone;

    this.salaForm = this.fb.group({
      nome: ['', Validators.required],
      descricao: [''],
      capacidade: [0, [Validators.required, Validators.min(1)]],
      fusoHorario: [userTimezone, Validators.required],
    });
  }

  ngOnInit(): void {
    this.salaId = this.activatedRoute?.snapshot?.paramMap?.get('salaId');
    if (this.salaId) {
      this.titulo = 'TITLEEDITARSALA';
      this.getSala(this.salaId);
    }
  }

  getSala(salaId: string): void {
    this.salasService.getSala(salaId).subscribe(
      (response) => {
        this.salaForm.patchValue({
          nome: response.nome,
          descricao: response.descricao,
          capacidade: response.capacidade,
          fusoHorario: response.fusoHorario,
        });
      },
      (error) => {
        this.snackBar.open('Erro ao recuperar a sala', '', {
          duration: 3000,
        });
      }
    );
  }

  onSubmit(): void {
    if (this.salaForm.valid) {
      if (this.salaId) {
        this.salasService
          .updateSala(this.salaId, this.salaForm.value)
          .subscribe(
            (response) => {
              this.snackBar.open('Sala atualizada com sucesso!', '', {
                duration: 3000,
              });
              this.router.navigate(['/salas']);
            },
            (error) => {
              this.snackBar.open('Erro ao atualizar a sala', '', {
                duration: 3000,
              });
            }
          );
      } else {
        this.salasService.createSala(this.salaForm.value).subscribe(
          (response) => {
            this.snackBar.open('Sala cadastrada com sucesso!', '', {
              duration: 3000,
            });
            this.router.navigate(['/salas']);
          },
          (error) => {
            this.snackBar.open('Erro ao cadastrar a sala', '', {
              duration: 3000,
            });
          }
        );
      }
    }
  }

  onCancel(): void {
    this.router.navigate(['/salas']);
  }
}
