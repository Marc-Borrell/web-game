import { Component, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-start',
  imports: [],
  templateUrl: './start.html',
  styleUrl: './start.scss',
})
export class Start {
  @ViewChild('imageCard') cardElement!: ElementRef;

  // Variables para los estilos
  cardTransform: string = '';
  mouseX: number = 0;
  mouseY: number = 0;

  onMouseMove(e: MouseEvent) {
    const card = this.cardElement.nativeElement;
    const rect = card.getBoundingClientRect();
    
    // Posición del mouse dentro de la carta
    this.mouseX = e.clientX - rect.left;
    this.mouseY = e.clientY - rect.top;
    
    const centerX = rect.width / 2;
    const centerY = rect.height / 2;

    // Cálculo de la rotación
    const rotateX = (centerY - this.mouseY) / 10;
    const rotateY = (this.mouseX - centerX) / 10;

    this.cardTransform = `rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
  }

  onMouseLeave() {
    // Resetear la posición suavemente
    this.cardTransform = 'rotateX(0deg) rotateY(0deg)';
  }
}
