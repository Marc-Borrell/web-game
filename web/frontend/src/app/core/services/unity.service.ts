import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class UnityService {
    
  private unityInstance: any = null;

  setInstance(instance: any) {
    this.unityInstance = instance;
  }

  getInstance() {
    return this.unityInstance;
  }

  sendMessage(gameObject: string, method: string, value: string) {
    if (!this.unityInstance) {
      console.warn('Unity no está listo todavía');
      return;
    }
    this.unityInstance.SendMessage(gameObject, method, value);
  }
}