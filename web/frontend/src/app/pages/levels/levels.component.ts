import { Component, OnInit } from '@angular/core';
import { Navbar } from '../../shared/components/navbar/navbar.component';
import { Footer } from '../../shared/components/footer/footer.component';
import { UnityService } from '../../core/services/unity.service';
import { Level } from '../../core/services/level.service';

@Component({
  selector: 'app-levels',
  imports: [Navbar, Footer],
  templateUrl: './levels.component.html',
  styleUrl: './levels.component.scss',
})
export class Levels implements OnInit {

  levels: { id: number, name: string }[] = [];
  constructor(
    private unityService: UnityService,
    private levelsService: Level
  ) {}

  ngOnInit() {
    this.levelsService.getLevels().subscribe(data => {
      this.levels = data;
    });
  }

  cargarNivel(levelName: string) {
    this.unityService.sendMessage('GameManager', 'LoadLevel', levelName);
  }

}
