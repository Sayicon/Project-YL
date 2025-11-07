/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   builtin_echo.c                                     :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/19 02:45:39 by caide-so          #+#    #+#             */
/*   Updated: 2025/05/19 04:12:40 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

int		is_all_n_flag(const char *str);
void	write_echo_args(char *arg);

int	builtin_echo(char **args)
{
	int	supress_nl;
	int	i;

	supress_nl = 0;
	i = 1;
	while (args[i] && is_all_n_flag(args[i]))
	{
		supress_nl = 1;
		i++;
	}
	while (args[i])
	{
		write_echo_args(args[i]);
		if (args[i + 1])
			write(STDOUT_FILENO, " ", 1);
		i++;
	}
	if (!supress_nl)
		write(STDOUT_FILENO, "\n", 1);
	return (0);
}

// Checks if a string is a valid -n flag.
// 1. Returns 1 if the string starts with '-' followed by only 'n's.
// 2. Returns 0 otherwise.
int	is_all_n_flag(const char *str)
{
	int	i;

	if (str[0] != '-' || str[1] == '\0')
		return (0);
	i = 1;
	while (str[i])
	{
		if (str[i] != 'n')
			return (0);
		i++;
	}
	return (1);
}

// Writes the string argument character by character to STDOUT.
// 1. Iterates through the string and writes each character.
void	write_echo_args(char *arg)
{
	int	i;

	i = 0;
	while (arg[i])
	{
		write(STDOUT_FILENO, &arg[i], 1);
		i++;
	}
}
