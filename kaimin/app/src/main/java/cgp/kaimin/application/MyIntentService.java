package cgp.kaimin.application;

import android.app.IntentService;
import android.content.Intent;

public class MyIntentService extends IntentService {

    private static final String SET_ACTION = "TIMER_FINISHED";

    public MyIntentService() {

        super("TimerIntentService");
    }

    // バックグラウンドでの処理
    @Override
    protected void onHandleIntent(Intent data) {

        Intent intent = new Intent();
        intent.putExtra("ALARM_FLAG", data.getIntExtra("ALARM_FLAG", 0));

        intent.setAction(SET_ACTION); // 指定したアクション名でBroadCastを起動
        sendBroadcast(intent);

    }
}
