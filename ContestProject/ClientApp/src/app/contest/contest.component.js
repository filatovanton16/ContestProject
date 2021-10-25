var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from '@angular/core';
let ContestComponent = class ContestComponent {
    constructor(dataService, cookieService) {
        this.dataService = dataService;
        this.cookieService = cookieService;
        this.initCode = "public static int MyMethod(int input) {\nreturn 1;\n}";
        this.isRunned = false;
        this.runTouched = false;
    }
    ngOnInit() {
        this.dataService.getTaskNames().subscribe((data) => {
            this.taskNames = data;
            this.userTaskCode = { userName: "", taskName: data[0], code: this.initCode };
            if (this.cookieService.check('userName')) {
                this.userTaskCode.userName = this.cookieService.get('userName');
            }
            this.dataService.getContestTask(this.userTaskCode.taskName).subscribe((data) => this.description = data);
        });
    }
    chooseTask() {
        this.dataService.getContestTask(this.userTaskCode.taskName).subscribe((data) => this.description = data);
    }
    saveResult() {
        this.runTouched = true;
        if (this.userTaskCode.userName == "")
            return;
        this.cookieService.set('userName', this.userTaskCode.userName);
        this.isRunned = true;
        this.info = "WAIT...";
        this.dataService.addUserTask(this.userTaskCode).subscribe((data) => { this.info = data; });
    }
};
ContestComponent = __decorate([
    Component({
        templateUrl: './contest.component.html',
        styleUrls: ['./contest.component.css']
    })
], ContestComponent);
export { ContestComponent };
//# sourceMappingURL=contest.component.js.map