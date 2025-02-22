import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateOrUpdateAgendamentoComponent } from './create-or-update-agendamento.component';

describe('CreateOrUpdateAgendamentoComponent', () => {
  let component: CreateOrUpdateAgendamentoComponent;
  let fixture: ComponentFixture<CreateOrUpdateAgendamentoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateOrUpdateAgendamentoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateOrUpdateAgendamentoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
