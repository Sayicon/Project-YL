/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   setup_signals_exec.c                               :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: marcudos <marcudos@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/06/03 18:03:13 by marcudos          #+#    #+#             */
/*   Updated: 2025/06/03 20:20:20 by marcudos         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

void	setup_signals_exec(void)
{
	t_sig	sa_int;
	t_sig	sa_quit;

	sa_int = setup_sigint_exec();
	sigaction(SIGINT, &sa_int, NULL);
	sa_quit = setup_sigquit_exec();
	sigaction(SIGQUIT, &sa_quit, NULL);
}

t_sig	setup_sigint_exec(void)
{
	t_sig	sa_int;

	sa_int.sa_handler = handle_sigint_exec;
	sigemptyset(&sa_int.sa_mask);
	sa_int.sa_flags = 0;
	return (sa_int);
}

void	handle_sigint_exec(int sig)
{
	if (sig == SIGINT)
	{
		g_signal_status = 130;
		write(1, "\n", 1);
	}
}

void	handle_sigquit_exec(int sig)
{
	if (sig == SIGQUIT)
	{
		g_signal_status = 131;
		write(1, "Quit (core dumped)\n", 19);
	}
}

t_sig	setup_sigquit_exec(void)
{
	t_sig	sa_quit;

	sa_quit.sa_handler = SIG_DFL;
	sigemptyset(&sa_quit.sa_mask);
	sa_quit.sa_flags = 0;
	return (sa_quit);
}
