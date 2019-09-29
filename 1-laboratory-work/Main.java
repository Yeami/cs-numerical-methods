import java.util.Scanner;

public class Main {

    public static void main(String[] args) {

        Scanner reader = new Scanner(System.in);

        System.out.print("Enter begining of interval: ");
        double startOfInterval = reader.nextDouble(); // Xn-1

        System.out.print("Enter end of interval: ");
        double endOfInterval = reader.nextDouble(); // Xn

        System.out.print("Enter precision of method: ");
        double precision = reader.nextDouble();

        System.out.println(java.lang.String.format("\nResult:\nX: %s", secant(precision, startOfInterval, endOfInterval)));
    }

    private static double secant(double precision, double startOfInterval, double endOfInterval) {

        int stepsCounter = 1;
        double x = startOfInterval;

        while(Math.abs(function(x)) > precision) {
            System.out.println(java.lang.String.format("\nStep #%d\nX: %s\nf(x): %s", stepsCounter, x, function(x)));
            stepsCounter += 1;

            x = endOfInterval - ((function(endOfInterval) * (endOfInterval - startOfInterval)) / (function(endOfInterval) - function(startOfInterval)));
            startOfInterval = endOfInterval;
            endOfInterval = x;
        }

        return x;
    }

    private static double function(double x) {
        return Math.pow((Math.E), x) - (2 * x) - 5;
    }
}