import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { AgendamentosComponent } from './pages/agendamentos/agendamentos.component';
import { SalasComponent } from './pages/salas/salas.component';
import { CreateOrUpdateSalaComponent } from './pages/create-or-update-sala/create-or-update-sala.component';
import { CreateOrUpdateAgendamentoComponent } from './pages/create-or-update-agendamento/create-or-update-agendamento.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'agendamentos', component: AgendamentosComponent },
  { path: 'salas', component: SalasComponent },
  { path: 'sala/:salaId', component: CreateOrUpdateSalaComponent },
  { path: 'sala', component: CreateOrUpdateSalaComponent },
  { path: 'agendamentos/salas/:salaId', component: AgendamentosComponent },
  {
    path: 'agendamento/create/:salaId',
    component: CreateOrUpdateAgendamentoComponent,
  },
  {
    path: 'agendamento/create',
    component: CreateOrUpdateAgendamentoComponent,
  },
  {
    path: 'agendamento/edit/:agendamentoId',
    component: CreateOrUpdateAgendamentoComponent,
  },
];
