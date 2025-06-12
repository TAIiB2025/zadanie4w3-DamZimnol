import { Component, OnInit } from '@angular/core';
import { ListaService } from '../lista.service';
import { Observable } from 'rxjs';
import { Film } from '../../models/film';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-lista',
  templateUrl: './lista.component.html',
  styleUrls: ['./lista.component.css'],
  imports: [FormsModule, CommonModule, RouterModule]
})
export class ListaComponent implements OnInit {
  dane$: Observable<Film[]> | undefined;
  filterText: string = ''; // tekst filtrowania

  constructor(private listaService: ListaService) {}

  ngOnInit(): void {
    this.getFilms();
  }

  getFilms(): void {
    this.dane$ = this.listaService.get(this.filterText);
  }

  onFilterChange(): void {
    this.getFilms();
  }
}
