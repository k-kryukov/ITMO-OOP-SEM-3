namespace IsuExtra.Exceptions;

using System;
using IsuExtra.Models;

public class TimeslotAlreadyOccupated : Exception
{
    public TimeslotAlreadyOccupated(LessonTime beginTime, Subject subject)
    : base($"{beginTime} is already occupated by {subject}") { }
}