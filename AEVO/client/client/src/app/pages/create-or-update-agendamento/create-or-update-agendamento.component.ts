import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { MatSnackBar } from '@angular/material/snack-bar';
import { AgendamentosService } from '../../services/agendamentos/agendamentos.service';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SalasService } from '../../services/salas/salas.service';
import { MatSelectModule } from '@angular/material/select';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-create-or-update-agendamento',
  templateUrl: './create-or-update-agendamento.component.html',
  styleUrls: ['./create-or-update-agendamento.component.css'],
  imports: [
    MatButtonModule,
    MatDialogModule,
    MatIconModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    CommonModule,
    MatTooltipModule,
    MatSelectModule,
    TranslatePipe,
  ],
})
export class CreateOrUpdateAgendamentoComponent implements OnInit {
  agendamentoForm: FormGroup;
  agendamentoId: string | null = null;
  titulo: string = 'TITLEADICIONARAGENDAMENTO';
  isLoading: boolean = false;
  salaId: string | null = null;
  salas: any[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private agendamentosService: AgendamentosService,
    private salasService: SalasService,
    private snackBar: MatSnackBar
  ) {
    const userTimezone = Intl.DateTimeFormat().resolvedOptions().timeZone;
    const usuarioId = localStorage.getItem('usuarioId');

    this.agendamentoForm = this.fb.group({
      salaId: ['', Validators.required],
      usuarioId: [usuarioId, Validators.required],
      inicio: ['', Validators.required],
      fim: ['', Validators.required],
      timezone: [userTimezone, Validators.required],
    });
  }

  ngOnInit(): void {
    this.getSalas();

    this.activatedRoute.paramMap.subscribe((params) => {
      const agendamentoId = params.get('agendamentoId');
      if (agendamentoId) {
        this.agendamentoId = agendamentoId;
        this.titulo = 'TITLEEDITORAGENDAMENTO';
        this.carregarAgendamento(agendamentoId);
      }

      const salaId = params.get('salaId');
      if (salaId) {
        this.salaId = salaId;
        this.agendamentoForm.patchValue({
          salaId: salaId,
        });
      }
    });
  }

  carregarAgendamento(agendamentoId: string): void {
    this.isLoading = true;
    this.agendamentosService.getAgendamento(agendamentoId).subscribe(
      (data) => {
        this.agendamentoForm.patchValue({
          salaId: data.sala?.id,
          usuarioId: data.usuario?.id,
          inicio: data.inicio,
          fim: data.fim,
          timezone: data.timezone,
        });
        this.isLoading = false;
      },
      (error) => {
        this.snackBar.open('Erro ao carregar o agendamento', '', {
          duration: 3000,
        });
        this.isLoading = false;
      }
    );
  }

  getSalas() {
    this.isLoading = true;
    this.salasService.getSalas().subscribe(
      (data) => {
        this.salas = data.length > 0 ? data : [];
        this.isLoading = false;
      },
      (error) => {
        this.isLoading = false;
        this.salas = [];
      }
    );
  }

  salvarAgendamento(): void {
    const agendamentoData = this.agendamentoForm.value;

    if (
      this.agendamentoForm.invalid ||
      (!this.isValidDate(agendamentoData.inicio) &&
        !this.isValidDate(agendamentoData.fim))
    ) {
      this.snackBar.open('Preenchimento invalido!', '', {
        duration: 3000,
      });
      return;
    }

    this.isLoading = true;

    if (this.agendamentoId) {
      this.agendamentosService
        .updateAgendamento(this.agendamentoId, {
          ...agendamentoData,
          agendamentoId: this.agendamentoId,
        })
        .subscribe(
          () => {
            this.snackBar.open('Agendamento atualizado com sucesso!', '', {
              duration: 3000,
            });
            this.router.navigate(['/agendamentos']);
          },
          (error) => {
            this.snackBar.open('Erro ao atualizar agendamento', '', {
              duration: 3000,
            });
            this.isLoading = false;
          }
        );
    } else if (agendamentoData.salaId) {
      this.agendamentosService
        .createAgendamento(agendamentoData.salaId, agendamentoData)
        .subscribe(
          () => {
            this.snackBar.open('Agendamento criado com sucesso!', '', {
              duration: 3000,
            });
            this.router.navigate(['/agendamentos']);
          },
          (error) => {
            this.snackBar.open('Erro ao criar agendamento', '', {
              duration: 3000,
            });
            this.isLoading = false;
          }
        );
    }
  }

  isValidDate(date: string): boolean {
    const regex =
      /^(?:\d{4})-(?:0[1-9]|1[0-2])-(?:0[1-9]|[12][0-9]|3[01])T(?:[01][0-9]|2[0-3]):(?:[0-5][0-9])(:[0-5][0-9])?$/;

    return regex.test(date);
  }

  voltar(): void {
    this.router.navigate(['/agendamentos']);
  }
}
