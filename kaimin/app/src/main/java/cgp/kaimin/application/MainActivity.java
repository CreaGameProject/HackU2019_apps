package cgp.kaimin.application;

import android.os.Bundle;

import com.google.android.material.bottomnavigation.BottomNavigationView;

import androidx.appcompat.app.AppCompatActivity;
import androidx.annotation.NonNull;

import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.util.Calendar;
import android.app.PendingIntent;
import android.app.AlarmManager;
import android.content.Intent;

public class MainActivity extends AppCompatActivity {
    private TextView textView;
    private  boolean flag=false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Button button=findViewById(R.id.button);

        textView=findViewById(R.id.text_view);

        button.setOnClickListener(new View.OnClickListener(){
            @Override
            public void onClick(View view){
                AlarmManager alarmManager = (AlarmManager) getSystemService(ALARM_SERVICE);
                alarmManager.set(AlarmManager.RTC_WAKEUP, setAlarmTime().getTimeInMillis(), setAlarmPendingIntent(1));
                if(flag){
                    textView.setText("Hello");
                    flag = false;
                }else{
                    textView.setText("World");
                    flag = true;
                }
            }
        });
    }

    private Calendar setAlarmTime() {

        Calendar calendar = Calendar.getInstance();
        calendar.setTimeInMillis(System.currentTimeMillis());
        calendar.add(Calendar.SECOND, 10);

        return calendar;
    }
    private PendingIntent setAlarmPendingIntent(int flags) {

        Intent intent = new Intent(getApplicationContext(), MyIntentService.class);
        intent.putExtra("ALARM_FLAG", flags);
        PendingIntent pendingIntent = PendingIntent.getService(getApplicationContext(), 0, intent, flags);

        return pendingIntent;
    }


}
