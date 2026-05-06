import { Component, inject } from '@angular/core';
import { Auth } from '../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class Home {

  private authService = inject(Auth);
  private router = inject(Router);

  public usuari: String = this.authService.getUser().name; 

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  ngOnInit() {
        //@ts-ignore
        createUnityInstance(document.querySelector("#unity-canvas"), {
          dataUrl: "/unity/Build/probaWeb.data",
          frameworkUrl: "/unity/Build/probaWeb.framework.js",
           codeUrl: "/unity/Build/probaWeb.wasm",
          streamingAssetsUrl: "StreamingAssets",
          companyName: "QQClan",
          productName: "NOM-Protocol",
          productVersion: "1.0"
        });
  }

}
