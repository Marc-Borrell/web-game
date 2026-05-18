import { Component, OnInit } from '@angular/core';
import { Navbar } from '../../shared/components/navbar/navbar.component';
import { Footer } from '../../shared/components/footer/footer.component';
import { RankingEntry, RankingService } from '../../core/services/ranking.service';
import { TimeFormatPipe } from '../../shared/pipes/time-format.pipe';
import { Score } from '../../core/services/score.service';

@Component({
  selector: 'app-ranking',
  imports: [Navbar, Footer, TimeFormatPipe],
  templateUrl: './ranking.component.html',
  styleUrl: './ranking.component.scss',
})
export class Ranking implements OnInit{
  ranking: RankingEntry[] = [];
  levelSeleccionat: number = 1;
  nivells: number[] = [1,2,3,4,5];
  nivellSecret: number = 6;                   
  mostrarSecret: boolean = false;
  carregant: boolean = false;

  constructor(
    private rankingService: RankingService,
    private scoreService: Score
  ) {}

  ngOnInit(): void {
    this.scoreService.getScoreUser().subscribe({
      next: (completats) => {
        //Mostrar boto nivell secret si s'ha completat
        this.mostrarSecret = completats.includes(this.nivellSecret)
      },
      error: (err) => console.error(err)
    });

    this.carregarRanking();
  }

  carregarRanking(): void {
    this.carregant = true;
    this.rankingService.getRanking(this.levelSeleccionat).subscribe({
      next: (data) => {
        this.ranking = data;
        this.carregant = false;
      },
      error: (err) => {
        console.error('Error carregant ranking: ' , err);
        this.carregant = false;
      }
    });
  }

}
