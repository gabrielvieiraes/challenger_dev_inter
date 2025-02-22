import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { Router } from '@angular/router';

import { SalasService } from '../../services/salas/salas.service';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTable, MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-sala',
  templateUrl: './salas.component.html',
  styleUrls: ['./salas.component.css'],
  imports: [
    MatTable,
    MatTableModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    CommonModule,
    MatTooltipModule,
    TranslatePipe,
  ],
})
export class SalasComponent implements OnInit {
  displayedColumns: string[] = [
    'nome',
    'descricao',
    'capacidade',
    'fusoHorario',
    'acoes',
  ];
  dataSource: any[] = [];
  isLoading = true;

  constructor(
    private salasService: SalasService,
    public dialog: MatDialog,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.getSalas();
  }

  getSalas() {
    this.isLoading = true;
    this.salasService.getSalas().subscribe(
      (data) => {
        this.dataSource = data.length > 0 ? data : [];
        this.isLoading = false;
      },
      (error) => {
        this.snackBar.open('Erro ao carregar as salas', '', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: ['error-snackbar'],
        });
        this.isLoading = false;
        this.dataSource = [];
      }
    );
  }

  createOrUpdateSala(salaId?: string) {
    if (salaId) {
      this.router.navigate(['/sala', salaId]);
    } else {
      this.router.navigate(['/sala']);
    }
  }

  openDeleteDialog(salaId: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { salaId },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'yes') {
        this.deleteSala(salaId);
      }
    });
  }

  visualizarAgendamentos(salaId: string): void {
    this.router.navigate(['/agendamentos/salas', salaId]);
  }

  private deleteSala(salaId: string) {
    this.salasService.deleteSala(salaId).subscribe(
      () => {
        this.snackBar.open('Sala excluída com sucesso', '', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: ['success-snackbar'],
        });
        this.getSalas();
      },
      (error) => {
        this.snackBar.open('Erro ao excluir a sala', '', {
          duration: 3000,
          verticalPosition: 'top',
          panelClass: ['error-snackbar'],
        });
      }
    );
  }
}
