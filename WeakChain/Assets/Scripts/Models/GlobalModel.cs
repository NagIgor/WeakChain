//модель для глобальных переменных, чтобы все остальные контроллеры могли иметь к ней доступ
public static class GlobalModel
{
    public const int BASE_TIMER = 30;
    public const int MIN_PLAYERS = 4;
    public const int MAX_PLAYERS = 12;
    
    public static int PlayersAmount;
}
