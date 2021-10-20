import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class LeaderboardDataService {

    private url = "/api/leaderboard";

    constructor(private http: HttpClient) {
    }

    getLeaderboard() {
        return this.http.get(this.url);
    }
}