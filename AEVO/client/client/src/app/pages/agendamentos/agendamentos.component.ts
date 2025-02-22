import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { AgendamentosService } from '../../services/agendamentos/agendamentos.service';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-agendamentos',
  templateUrl: './agendamentos.component.html',
  styleUrls: ['./agendamentos.component.css'],
  imports: [
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    MatIconModule,
    MatSortModule,
    MatPaginatorModule,
    CommonModule,
    TranslatePipe,
  ],
})
export class AgendamentosComponent implements OnInit {
  displayedColumns: string[] = ['sala', 'inicio', 'fim', 'actions'];
  dataSource: MatTableDataSource<any> = new MatTableDataSource();
  salaId: string | null = null;

  constructor(
    private agendamentoService: AgendamentosService,
    private router: Router,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {
    const accessToken = localStorage.getItem('accessToken');
    if (!accessToken) {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit() {
    this.salaId = this.activatedRoute?.snapshot?.paramMap?.get('salaId');
    if (this.salaId) {
      this.loadAgendamentosDaSala(this.salaId);
    } else {
      this.loadAgendamentos();
    }
  }

  loadAgendamentos() {
    const usuarioId = localStorage.getItem('usuarioId');

    this.agendamentoService
      .getAgendamentosUsuario({ usuarioId: usuarioId })
      .subscribe((agendamentos) => {
        this.dataSource.data = agendamentos;
      });
  }

  loadAgendamentosDaSala(salaId: string) {
    this.agendamentoService
      .getAgendamentosPorSala(salaId)
      .subscribe((agendamentos) => {
        this.dataSource.data = agendamentos;
      });
  }

  editAgendamento(id: string) {
    this.router.navigate([`agendamento/edit/${id}`]);
  }

  deleteAgendamento(id: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '300px',
      data: { message: 'Tem certeza que deseja excluir este agendamento?' },
    });

    const usuarioId = localStorage.getItem('usuarioId');

    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'yes' && usuarioId) {
        this.agendamentoService.deleteAgendamento(id, usuarioId).subscribe(
          () => {
            this.loadAgendamentos();
          },
          (error) => {
            this.snackBar.open('Erro ao excluir o agendamento', '', {
              duration: 3000,
            });
          }
        );
      }
    });
  }

  addAgendamento() {
    if (this.salaId) {
      this.router.navigate(['/agendamento/create', this.salaId]);
    } else {
      this.router.navigate(['/agendamento/create']);
    }
  }
}
