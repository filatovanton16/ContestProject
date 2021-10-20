var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from '@angular/core';
let ContestComponent = class ContestComponent {
    constructor(dataService) {
        this.dataService = dataService;
        this.userTaskCode = { name: "", task: "", code: "", inputParameter: 0, outputParameter: 0 };
        this.contestTask = { task: "", description: "", inputParameter: 0, outputParameter: 0 };
        this.userName = "";
        this.selectedTask = "TheSimpliestTask";
        this.code = "public static int MyMethod(int input) {\nreturn 5 * input;\n}";
        this.isRunned = false;
    }
    ngOnInit() {
        this.dataService.getTaskNames().subscribe((data) => this.taskNames = data);
        this.dataService.getContestTask(this.selectedTask).subscribe((data) => this.contestTask = data);
    }
    chooseTask() {
        this.dataService.getContestTask(this.selectedTask).subscribe((data) => this.contestTask = data);
    }
    saveResult() {
        this.isRunned = true;
        this.info = "WAIT...";
        this.userTaskCode = { name: this.userName, task: this.selectedTask, code: this.code, inputParameter: this.contestTask.inputParameter, outputParameter: this.contestTask.outputParameter };
        this.dataService.addUserTask(this.userTaskCode).subscribe((data) => {
            if (data) {
                this.info = "!!!SUCCESS!!!";
            }
            else {
                this.info = "WRONG :(";
            }
        });
    }
};
ContestComponent = __decorate([
    Component({
        templateUrl: './contest.component.html'
    })
], ContestComponent);
export { ContestComponent };
//# sourceMappingURL=contest.component.js.map